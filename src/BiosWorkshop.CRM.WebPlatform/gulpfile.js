// variables
var libraryList = [
    { name: "bootstrap", path: "./node_modules/bootstrap/dist/**/", src: ["*.css*", "*.js*"] },
    { name: "jquery", path: "./node_modules/jquery/dist/**/", src: ["*.css*", "*.js*"] },
    { name: "popper.js", path: "./node_modules/popper.js/dist/**/", src: ["*.css*", "*.js*"] }
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