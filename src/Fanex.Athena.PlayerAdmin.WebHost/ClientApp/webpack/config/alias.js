var projectRoot = require('./path').projectRoot;

var alias = {
    'vendor-materialize': projectRoot + "/ClientApp/src/jquery-pages/js/vendors/materialize/materialize.js",
    common: projectRoot + "/ClientApp/src/jquery-pages/js/common",
    lib: projectRoot + "/ClientApp/src/jquery-pages/js/lib",
    extensions: projectRoot + "/ClientApp/src/jquery-pages/js/extensions",
    components: projectRoot + "/ClientApp/src/jquery-pages/js/components",
    helpers: projectRoot + "/ClientApp/src/jquery-pages/js/helpers",   
};

module.exports = alias;