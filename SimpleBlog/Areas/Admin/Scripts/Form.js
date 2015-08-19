$(document).ready(function() {

    // cerate a dialog box confirm deletion
    $("a[data-post]").click(function(e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("post");

        //if there is a message and they cancel on dialog box
        if (message && !confirm(message))
            return;

        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .appendTo(document.body)
            .submit();
    });
});