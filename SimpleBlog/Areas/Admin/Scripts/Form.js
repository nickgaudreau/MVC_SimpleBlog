$(document).ready(function() {

    // cerate a dialog box confirm deletion
    $("a[data-post]").click(function(e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("post");

        //if there is a message and they cancel on dialog box
        if (message && !confirm(message))
            return;

        // COPY/APPEND TO THEN CAPTURE AND POST TO THE REQUEST SO DELETION IS POSSIBLE (NO ERROR THROWN FOR NO ANTI FORGERY TOKEN)
        // get the input tag of the anti forge form
        var antiForgeryToken = $("#anti-forgery-form input");
        // add a hidden input, set that hideen input name to what the token had and his val
        var antiForgeryInput = $("<input type='hidden'").attr("name", antiForgeryToken.attr("name")).val(antiForgeryToken.val());
        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .append(antiForgeryInput)
            .appendTo(document.body)
            .submit();
    });

    // loop data-slug attr element
    $("[data-slug]").each(function() {
        var $this = $(this); 
        var $sendSlugFrom = $($this.data("slug")); // store its value

        $sendSlugFrom.keyup(function() { // everytime a key is press we produced a slug of that data
            var slug = $sendSlugFrom.val();
            slug = slug.replace(/[^a-zA-Z0-9\s]/g, ""); // remove special char for empty
            slug = slug.toLowerCase();
            slug = slug.replace(/\s+/g, "-"); // remove spaces for -

            if (slug.charAt(slug.length - 1) == "-")
                slug = slug.substr(0, slug.length - 1);

            $this.val(slug);
        });
    });

});