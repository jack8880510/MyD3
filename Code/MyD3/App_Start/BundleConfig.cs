using System.Web;
using System.Web.Optimization;

namespace DemoSite
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var urlTransform = new CssRewriteUrlTransform();

            //包含组件样式库
            bundles.Add(new StyleBundle("~/bundles/Components/style")
                      .Include("~/asset/Components/bower_components/animate.css/animate.css")
                      .Include("~/asset/Components/bootgrid/jquery.bootgrid.css")
                      .Include("~/asset/Components/bower_components/bootstrap-sweetalert/lib/sweet-alert.css")
                      .Include("~/asset/Components/bower_components/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css")
                      .Include("~/asset/Components/bower_components/bootstrap-select/dist/css/bootstrap-select.css")
                      .Include("~/asset/Components/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.css", urlTransform));
            bundles.Add(new StyleBundle("~/bundles/Components/style/app1").Include("~/asset/Components/app.min.1.css", urlTransform));
            bundles.Add(new StyleBundle("~/bundles/Components/style/app2").Include("~/asset/Components/app.min.2.css", urlTransform));

            //包含站点样式库
            bundles.Add(new StyleBundle("~/bundles/site/styles").Include("~/asset/Site/Styles/site.css"));

            //包含MyD3基本库
            bundles.Add(new ScriptBundle("~/bundles/jack-tool")
                .Include("~/asset/JackTool/jackTool.js")
                .Include("~/asset/JackTool/jackTool.tools*")
                .Include("~/asset/JackTool/jackTool.logininfo.js")
                //LoginInfo必须在Security之前初始化，因为他们之间有依赖关系
                .Include("~/asset/JackTool/jackTool.security.js"));

            //包含第三方基本库
            bundles.Add(new ScriptBundle("~/bundles/Components/Base")
                .Include("~/asset/Components/jquery/jquery-{version}.js")
                .Include("~/asset/Components/jquery-validate/jquery.validate.js")
                .Include("~/asset/Components/jquery-form/jquery.form.js")
                .Include("~/asset/Components/bower_components/bootstrap/dist/js/bootstrap.js")
                .Include("~/asset/Components/Moment/moment-with-locales.js")
                .Include("~/asset/Components/functions.js")
                .Include("~/asset/Components/js-render/jsrender.js")
                .Include("~/asset/Components/underscore/underscore.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/nicescroll").Include("~/asset/Components/bower_components/jquery.nicescroll/jquery.nicescroll.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/Waves").Include("~/asset/Components/bower_components/Waves/dist/waves.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/SweetAlert").Include("~/asset/Components/bower_components/bootstrap-sweetalert/lib/sweet-alert.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/bootgrid").Include("~/asset/Components/bootgrid/jquery.bootgrid.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/bootstrap-growl").Include("~/asset/Components/bootstrap-growl/bootstrap-growl.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/bootstrap-select").Include("~/asset/Components/bower_components/bootstrap-select/dist/js/bootstrap-select.js")
                .Include("~/asset/Components/bower_components/bootstrap-select/dist/js/i18n/defaults-zh_CN.js"));
            bundles.Add(new ScriptBundle("~/bundles/Components/datetimePicker").Include("~/asset/Components/bower_components/eonasdan-bootstrap-datetimepicker/src/js/bootstrap-datetimepicker.js"));

            //启用压缩
            //BundleTable.EnableOptimizations = true;
        }
    }
}
