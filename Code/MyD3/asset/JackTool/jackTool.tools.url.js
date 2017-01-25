(function (jackTool) {
    //如果jackTool没有被初始化，那么抛出异常
    if (!jackTool) {
        throw new Error('jackTool.Net requires jackTool to be loaded first');
    }

    var url = {
        area: null,
        controller: null,
        action: null,
        pageTreePath: null
    };

    url.set = function (area, controller, action) {
        this.area = area;
        this.controller = controller;
        this.action = action;
    }

    url.setPageTreePath = function (pageTreePath) {
        if (pageTreePath) {
            if (pageTreePath.lastIndexOf("/") == pageTreePath.length - 1) {
                pageTreePath = pageTreePath.substr(0, pageTreePath.length - 1);
            }
        }
        this.pageTreePath = pageTreePath;
    }

    url.getPageTreePath = function () {
        if (this.pageTreePath) {
            return this.pageTreePath.split("/");
        }
        return [];
    }

    url.getArea = function () {
        return this.area;
    }

    url.getController = function () {
        return this.controller;
    }

    url.getAction = function () {
        return this.action;
    }

    jackTool.url = url;
})(window.jackTool.tools);