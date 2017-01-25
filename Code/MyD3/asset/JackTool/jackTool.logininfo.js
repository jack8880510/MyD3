(function (window, jackTool) {
    //如果jackTool没有被初始化，那么抛出异常
    if (!jackTool) {
        throw new Error('LoginInfo requires jackTool to be loaded first');
    }

    function get() {
        var loginInfo = JSON.parse(window.sessionStorage.getItem("login_info"));
        return loginInfo;
    }

    var logininfo = {};

    logininfo.set = function (userData) {
        if (userData) {
            window.sessionStorage.setItem("login_info", JSON.stringify(userData));
        }
    }

    logininfo.getUserName = function () {
        var loginInfo = get();
        if (loginInfo) {
            return loginInfo.User.UserData.DisplayName;
        }
        return null;
    }

    logininfo.getAuthorization = function () {
        var loginInfo = get();
        if (loginInfo) {
            return loginInfo.AllAuthorization;
        }
        return null;
    }

    jackTool.logininfo = logininfo;
})(window, window.jackTool);