$(document).ready(function () {
    setFieldVisibility();

    $("#SelectedApplicationType").change(function () {
        setFieldVisibility();
    });

    function setFieldVisibility() {
        if ($("#SelectedApplicationType").val() == 'Email') {
            $(".toAddresses").removeClass("hidden");
        } else {
            $(".toAddresses").addClass("hidden");
        }
    }
});
