(function (jackTool) {
    //如果jackTool没有被初始化，那么抛出异常
    if (!jackTool) {
        throw new Error('jackTool.View requires jackTool to be loaded first');
    }

    var view = {};

    view.build_fg_datetime_picker = function (context) {
        context.find('.date-time-picker').datetimepicker({
            format: "YYYY年MM月DD日 HH时mm分"
        });
        context.find('.time-picker').datetimepicker({
            format: "HH时mm分"
        });
        context.find('.date-picker').datetimepicker({
            format: "YYYY年MM月DD日"
        });
    }

    /**
    * 初始化选择框控件
    */
    view.build_select = function (context) {
        context.find(".selectpicker").selectpicker();
    }

    /**
    * 初始化FG INPUT
    */
    view.build_fg_input = function (object) {
        /*
         * Text Feild
         */
        if (object.find('.fg-line')[0]) {
            object.find('.fg-line').on('focus', '.form-control', function () {
                $(this).closest('.fg-line').addClass('fg-toggled');
            })

            object.find('.fg-line').on('blur', '.form-control', function () {
                var p = $(this).closest('.form-group');
                var i = p.find('.form-control').val();

                if (p.hasClass('fg-float')) {
                    if (i.length == 0) {
                        $(this).closest('.fg-line').removeClass('fg-toggled');
                    }
                }
                else {
                    $(this).closest('.fg-line').removeClass('fg-toggled');
                }
            });
        }

        if (object.find('.fg-float')[0]) {
            object.find('.fg-float .form-control').each(function () {
                var i = $(this).val();

                if (!i.length == 0) {
                    $(this).closest('.fg-line').addClass('fg-toggled');
                }

            });
        }
    }

    /**
    * 向jqValidate注入带日期的时间格式验证方法
    */
    view.build_validate_date_method = function () {
        //注入用于时间验证的jqValidate方法
        $.validator.methods.local_date = function (value, element) {
            return this.optional(element) || moment(value, "YYYY年MM月DD日 HH时mm分ss秒").isValid();
        }
    }

    /**
    * 向jqValidate注入时间格式验证方法
    */
    view.build_validate_time_method = function () {
        $.validator.methods.local_time = function (value, element) {
            return this.optional(element) || moment(value, "HH时mm分ss秒").isValid();
        }
    }

    /**
    * 向jqValidate注入带日期的时间最大值验证方法
    */
    view.build_validate_max_date_method = function () {
        $.validator.methods.max_date = function (value, element, param) {
            return this.optional(element) || moment(value, "YYYY年MM月DD日 HH时mm分ss秒") <= moment(param, "YYYY年MM月DD日 HH时mm分ss秒");
        }
    }

    /**
    * 向jqValidate注入带日期的时间最小值验证方法
    */
    view.build_validate_min_date_method = function () {
        $.validator.methods.min_date = function (value, element, param) {
            return this.optional(element) || moment(value, "YYYY年MM月DD日 HH时mm分ss秒") >= moment(param, "YYYY年MM月DD日 HH时mm分ss秒");
        }
    }

    /**
    * 向jqValidate注入时间最大值验证方法
    */
    view.build_validate_max_time_method = function () {
        $.validator.methods.max_time = function (value, element, param) {
            return this.optional(element) || moment(value, "HH时mm分ss秒") <= moment(param, "HH时mm分ss秒");
        }
    }

    /**
    * 向jqValidate注入时间最小值验证方法
    */
    view.build_validate_min_time_method = function () {
        $.validator.methods.min_time = function (value, element, param) {
            return this.optional(element) || moment(value, "HH时mm分ss秒") >= moment(param, "HH时mm分ss秒");
        }
    }

    /**
    * 向jqValidate注入正则表达式验证方法
    */
    view.build_validate_regex_method = function () {
        $.validator.methods.regex = function (value, element, param) {
            return this.optional(element) || new RegExp(param.trim()).test(value);
        }
    }

    /**
    * 向jqValidate注入所有日期相关的验证方法
    */
    view.build_validate_all_date_method = function () {
        view.build_validate_date_method();
        view.build_validate_time_method();
        view.build_validate_max_date_method();
        view.build_validate_min_date_method();
        view.build_validate_max_time_method();
        view.build_validate_min_time_method();
    }

    jackTool.view = view;
})(window.jackTool.tools);




