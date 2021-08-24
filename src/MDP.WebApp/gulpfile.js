// variables
var libs = [
    { name: "bootstrap", value: "./node_modules/bootstrap/dist/**/*.*" }
];

// require
var gulp = require("gulp");
var rimraf = require("rimraf");

// task
gulp.task("libs-clean", function (cb) {
    rimraf("./wwwroot/lib/", cb);
});

gulp.task("libs-copy", function (done) {
    libs.forEach(function (item) {
        gulp.src(item.value)
            .pipe(gulp.dest("./wwwroot/lib/" + item.name));
        done()
    });
});

gulp.task("publish", gulp.series("libs-clean", "libs-copy"));