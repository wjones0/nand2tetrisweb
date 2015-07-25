

angular.module('nand2tetris').factory('fileProcessingFactory', ['$http', function ($http) {

    var urlBase = '/HardwareSimulator/';
    var fileProcessingFactory = {};

    fileProcessingFactory.ParseFile = function (fileId) {
        return $http.post(urlBase + 'ParseFile', { id: fileId })
    };

    fileProcessingFactory.ProcessChip = function (fileId, inputs, inputvalues) {
        return $http.post(urlBase + 'ProcessChip', { fileid: fileId, inputIDs: inputs, inputVals: inputvalues });
    };


    return fileProcessingFactory;

}]);