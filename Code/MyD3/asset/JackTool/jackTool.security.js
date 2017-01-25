(function (window, jackTool, logininfo) {
    //如果jackTool没有被初始化，那么抛出异常
    if (!jackTool) {
        throw new Error('Security requires jackTool to be loaded first');
    } else if (!logininfo) {
        throw new Error('Security requires LoginInfo to be loaded first');
    }

    var security = {};

    security.logininfo = logininfo;
    security.jackTool = jackTool.tools;

    /**
    * 获取未授权的元素信息
    */
    function getNonAuthorizaEL() {
        //获取当前登录用户所有的未授权信息
        var allAuthorization = security.logininfo.getAuthorization();
        if (!allAuthorization) {
            //无授权信息时返回空
            return null;
        }

        //获取当前页面关联的未授权节点
        var blockedPageElement = getPageElements(allAuthorization.Non_PageElementAuthorizations);

        _.each(blockedPageElement, function (item, index, list) {
            var el = $(item.Selector);
            if (el[0]) {
                switch (item.BlockMethod) {
                    default:
                        el.hide();
                        break;
                }
            }
        });
    }

    /**
    * 获取本页面相关的授权信息集合
    */
    function getPageElements(authorization) {
        if (!authorization) {
            return null;
        }
        if (!(authorization instanceof Array)) {
            return null;
        }

        //尝试从URL工具中获取当前页面的TreePath
        var treePath = security.jackTool.url.getPageTreePath();

        var currentPageElements = _.filter(authorization, function (item) {
            return _.contains(treePath, item.PageID);
        });

        return currentPageElements;
    }

    function createPageElementTreeItem(currentPage, pages) {
        //判断当前页面节点
    }

    security.parse = function () {
        getNonAuthorizaEL();
    }

    jackTool.security = security;
})(window, window.jackTool, window.jackTool.logininfo);