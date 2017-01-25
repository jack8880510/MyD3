(function (jackTool) {
    //如果jackTool没有被初始化，那么抛出异常
    if (!jackTool) {
        throw new Error('jackTool.Net requires jackTool to be loaded first');
    }
    var net = {};

    function json(url, data, callback, method) {
        $.ajax({
            //请求的地址
            url: url,
            //请求体类型
            contentType: "application/json",
            //服务器参数
            data: JSON.stringify(data),
            //数据类型 json xml html script jsonp text
            dataType: "json",
            //请求类型 post/get
            type: method,
            //超时时间（毫秒）
            timeout: 1000 * 60,

            //成功回调function
            success: function (result) {
                if (callback) {
                    callback(result)
                } else {
                    alert(JSON.stringify(result));
                }
            },
            //失败回调function
            error: function (xmlHttpRequest) {
                var result = {
                    RCode: xmlHttpRequest.status,
                    RMsg: null
                };

                switch (xmlHttpRequest.status) {
                    case 403:
                        result.RMsg = "由于权限不够，您的请求被拒绝了";
                        break;
                    case 404:
                        result.RMsg = "请求地址错误，无法找到服务器";
                        break;
                    case 500:
                        result.RMsg = "服务器内部错误，请联系管理员";
                        break;
                    default:
                        result.RMsg = "未知错误，请联系管理员";
                        break;
                }

                if (xmlHttpRequest.getResponseHeader("statusText")) {
                    result.RMsg = decodeURI(xmlHttpRequest.getResponseHeader("statusText"));
                }

                if (callback) {
                    callback(result);
                } else {
                    alert(JSON.stringify(result));
                }
            },
            cache: false,

            //options: null,
            //回调上下文，如果不指定则为options，如果指定则使用context作为上下文
            //context: null,
            //是否异步，默认为true慎用false
            //async: true,
            //dataType 为 script 和 jsonp 时默认为 false。设置为 false 将不缓存此页面。
            //是否触发全局Ajax事件
            //global: true,
            //请求前function
            //beforeSend: null,
            //ajax返回前预处理function
            //dataFilter: null,
            //完成回调function
            //complete: null,
        });
    }

    net.post = function (url, data, callback) {
        json(url, data, callback, "post");
    }

    net.get = function (url, callback) {
        json(url, null, callback, "GET");
    }

    net.delete = function (url, data, callback) {
        json(url, data, callback, "DELETE");
    }

    net.put = function (url, data, callback) {
        json(url, data, callback, "PUT");
    }

    jackTool.net = net;
})(window.jackTool.tools);