

angular.module('nand2tetris').factory('sourceFileFactory', ['$http', function ($http) {

    var urlBase = '/api/SourceFiles/';
    var sourceFileFactory = {};

    sourceFileFactory.GetSourceFiles = function () {
        return $http.get(urlBase);
    };

    sourceFileFactory.NewFile = function (filename) {
        return $http.post(urlBase, { FileName: filename });
    };

    sourceFileFactory.SaveFile = function (fileId, fileName, fileBody) {
        return $http.put(urlBase + fileId, { id: fileId, FileName: fileName, FileBody: fileBody })
    };


    return sourceFileFactory;

}]);