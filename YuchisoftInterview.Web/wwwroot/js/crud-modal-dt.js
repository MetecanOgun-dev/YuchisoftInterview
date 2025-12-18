// wwwroot/js/crud-modal-dt.js
// Reusable CRUD: DataTables + single modal + AJAX (GET modal, POST save, POST delete)

window.CrudPage = (function () {
    function getToken() {
        const el = document.querySelector('meta[name="csrf-token"]');
        return el ? el.getAttribute('content') : '';
    }

    function toastSuccess(title) {
        if (!window.Swal) return;
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'success',
            title: title || 'Success',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }

    function toastError(title) {
        if (!window.Swal) return;
        Swal.fire({ icon: 'error', title: title || 'Error' });
    }

    function confirmDelete() {
        if (!window.Swal) return Promise.resolve(confirm('Delete?'));
        return Swal.fire({
            icon: 'warning',
            title: 'Silinsin mi?',
            text: 'Bu işlem geri alınamaz.',
            showCancelButton: true,
            confirmButtonText: 'Sil',
            cancelButtonText: 'Vazgeç'
        }).then(r => r.isConfirmed);
    }

    function defaultActionsHtml(id) {
        return `
      <button type="button" class="btn btn-sm btn-warning crud-edit" data-id="${id}">Düzenle</button>
      <button type="button" class="btn btn-sm btn-danger crud-delete" data-id="${id}">Sil</button>
    `;
    }

    function init(cfg) {
        const table = document.querySelector(cfg.tableSelector);
        const modalEl = document.querySelector(cfg.modalSelector);
        const modalBody = document.querySelector(cfg.modalBodySelector);
        const createBtn = document.querySelector(cfg.createBtnSelector);

        if (!table) throw new Error('CrudPage: table not found');
        if (!modalEl) throw new Error('CrudPage: modal not found');
        if (!modalBody) throw new Error('CrudPage: modal body not found');

        const bsModal = new bootstrap.Modal(modalEl);

        modalEl.addEventListener('hidden.bs.modal', () => {
            modalBody.innerHTML = '';
        });

        // DataTables
        const dt = new DataTable(cfg.tableSelector, cfg.dataTableOptions || {
            pageLength: 10,
            order: [[0, 'asc']],
        });

        // Create button
        if (createBtn) {
            createBtn.addEventListener('click', () => openModal(0));
        }

        // Event delegation for edit/delete
        table.addEventListener('click', (e) => {
            const editBtn = e.target.closest('.crud-edit');
            if (editBtn) {
                openModal(editBtn.dataset.id);
                return;
            }

            const delBtn = e.target.closest('.crud-delete');
            if (delBtn) {
                const id = delBtn.dataset.id;
                handleDelete(id);
                return;
            }
        });

        async function openModal(id) {
            const url = cfg.getModalUrl + `?id=${id}`;
            const resp = await fetch(url);
            modalBody.innerHTML = await resp.text();
            wireSaveForm();
            bsModal.show();
        }

        function wireSaveForm() {
            const form = modalBody.querySelector(cfg.formSelector || 'form');
            if (!form) return;

            form.addEventListener('submit', async (e) => {
                e.preventDefault();

                const fd = new FormData(form);
                const tokenInput = form.querySelector('input[name="__RequestVerificationToken"]');
                const token = tokenInput ? tokenInput.value : getToken();

                const resp = await fetch(cfg.saveUrl, {
                    method: 'POST',
                    headers: { 'RequestVerificationToken': token },
                    body: fd
                });

                if (resp.redirected && resp.url) {
                    window.location.href = resp.url;
                    return;
                }

                if (!resp.ok) {
                    // validation partial
                    const html = await resp.text();
                    modalBody.innerHTML = html;
                    wireSaveForm();
                    return;
                }

                const data = await resp.json(); // expected: { mode, id, ... }

                const rowData = cfg.mapToRow(data);
                if (!rowData) {
                    toastError('mapToRow returned empty');
                    return;
                }

                if (data.mode === 'create') {
                    const node = dt.row.add(rowData).draw(false).node();
                    if (node) node.setAttribute('data-id', data.id);
                    toastSuccess(cfg.createToastText || 'Kayıt eklendi');
                } else {
                    const tr = table.querySelector(`tr[data-id="${data.id}"]`);
                    if (tr) dt.row(tr).data(rowData).draw(false);
                    toastSuccess(cfg.updateToastText || 'Kayıt güncellendi');
                }

                bsModal.hide();
            });
        }

        async function handleDelete(id) {
            const ok = await confirmDelete();
            if (!ok) return;

            const token = getToken();

            const resp = await fetch(cfg.deleteUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'RequestVerificationToken': token
                },
                body: new URLSearchParams({ id })
            });

            if (resp.redirected && resp.url) {
                window.location.href = resp.url;
                return;
            }

            if (!resp.ok) {
                const txt = await resp.text();
                console.log('DELETE ERROR:', resp.status, txt);
                toastError(`Silme başarısız (${resp.status})`);
                return;
            }

            const data = await resp.json(); // expected: { ok:true, id }
            const tr = table.querySelector(`tr[data-id="${data.id}"]`);
            if (tr) dt.row(tr).remove().draw(false);

            toastSuccess(cfg.deleteToastText || 'Silindi');
        }

        return { dt, openModal };
    }

    return {
        init,
        actionsHtml: defaultActionsHtml
    };
})();
