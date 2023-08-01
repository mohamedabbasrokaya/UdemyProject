var updatedRow;

$(document).ready(function () {
    // handel bootstrap modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this)
        var modal = $('#Modal');
        modal.find('#ModalLabel').text(btn.data('title'));

        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr')
        }

        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                // show error message
            }
        })

        modal.modal('show');
    })

    // handel toggle status button
    $('body').delegate('.js-toggle-status', 'click', function () {
        var btn = $(this)
        $.post({
            url: btn.data('url'),
            data: {
                '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (lastUpdatedOn) {
                var status = btn.parents('tr').find('.js-status');
                var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                status.text(newStatus).toggleClass('bg-danger bg-success')
                btn.parents('tr').find('.js-updated-on').html(lastUpdatedOn);

            }
        })
    })
})


function onModalBegin() {
    $('body :submit').attr('disabled', 'disabled');
}


function onModalSuccess(item) {

    var modal = $('#Modal');
    modal.modal('hide');

    if (updatedRow === undefined) {
        $("tbody").append(item);
    } else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }
}


function onModalComplete() {
    $('body :submit').removeAttr('disabled');
}

