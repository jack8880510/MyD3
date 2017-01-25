(function (window) {
    /**
    * DS对象构造
    */
    var jackTool = function jackTool() {
        var instance = {
            tools: {},      //初始化工具模型
        };

        /**
        * 初始化处理
        */
        function initialize() {
            //设置当时区
            moment.locale("zh-cn");

            //返回当前DS工具对象
            return instance;
        }

        return initialize();
    }

    window.jackTool = jackTool();
})(window);