(function ($) {
    $.fn.clearbox = function (command) {
        if (command == "value") {
            var aggregate = "";
            $(this).each(function () {
                var stored = $(this).attr("data-orig-value");
                aggregate += "," + (stored == $(this).val() ? "" : $(this).val());
            });
            return aggregate.substr(1);
        }
        else {
            return this.each(function () {
                if (this.toString() != "[object HTMLInputElement]" || (this.type != "text" && this.type != "password"))
                    return undefined;

                var text = $(this).attr("data-orig-value", $(this).val());
                var password = $(this);
                var isPassword = this.type.toLowerCase() == "password";
                if (isPassword) {
                    text.hide();
                    text = $("<input type='text'/>").val(text.val());
                    text.insertBefore($(this));
                }

                text.css("color", "#AAA");

	            var orig = text.val();

                text.on("focus", function () {
                    if (isPassword) {
                        text.hide();
                        password.show().val("").css("color", "#000").focus();
                    } else if (text.val() == orig)
                        text.css("color", "#000").val("");
                });
                text.on("blur", function () {
                    if (!isPassword && text.val() == "")
                        text.css("color", "#AAA").val(orig);
                });

                if (isPassword) {
                    password.on("blur", function () {
                        if (password.val() == "") {
                            password.hide();
                            text.show();
                        }
                    });
                }

                return undefined;
            });
        }
    };
})(jQuery);