// variables
var libraryList = [
    { name: "admin-lte", path: "./node_modules/admin-lte/dist/**/", src: ["*.css*", "*.js*", "*.png", "*.jpg"] },
    { name: "bootstrap", path: "./node_modules/bootstrap/dist/**/", src: ["*.css*", "*.js*"] },
    { name: "jquery", path: "./node_modules/jquery/dist/**/", src: ["*.js*"] },
    { name: "fontawesome-free", path: "./node_modules/@fortawesome/fontawesome-free/**/", src: ["*.css*", "*.js*"] },
    { name: "fontawesome-free", path: "./node_modules/@fortawesome/fontawesome-free/*webfonts*/", src: ["*"] }
];

// require
var gulp = require("gulp");
var rimraf = require("rimraf");

// task
gulp.task("lib-clean", function (cb) {
    rimraf("./wwwroot/lib/", cb);
});

gulp.task("lib-copy", function (done) {
    libraryList.forEach(function (library) {
        library.src.forEach(function (src) {
            gulp.src(library.path + src).pipe(gulp.dest("./wwwroot/lib/" + library.name));
        });
        done();
    });
});

gulp.task("publish", gulp.series("lib-clean", "lib-copy"));