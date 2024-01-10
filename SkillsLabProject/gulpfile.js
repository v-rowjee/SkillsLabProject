/// <binding BeforeBuild='ts-default' ProjectOpened='ts-default' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
let ts = require('gulp-typescript');

let tsProject = ts.createProject('tsconfig.json');

gulp.task('ts-default', function () {
    // place code for your default task here
    return tsProject.src().pipe(tsProject()).js.pipe(gulp.dest("Scripts/dist/"));
});

gulp.task('match:ts-default', async function () {
    gulp.watch('Scripts/src/Widget/*.ts', gulp.series('ts-default'));
});