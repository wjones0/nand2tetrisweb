



angular.module('nand2tetris').controller('n2t-fileCtrl', ['$scope', '$http', function ($scope, $http) {

    $scope.hi = 'yo';
    $scope.fileExtensionFilter = ".hdl";

    $scope.ListFiles = function () {
        $http.get('/api/SourceFiles', {})
        .success(function (data, status, headers, config) {
            $scope.sourceFileData = data;
        })
        .error(function (data, status, headers, config) {

        });
    };


    $scope.newFileSubmit = function () {
        $http.post('/api/SourceFiles', { FileName: $scope.newFileName })
            .success(function (data, status, headers, config) {
                $scope.ListFiles();
            })
            .error(function (data, status, headers, config) {

            });

    };

    $scope.saveFile = function () {
        $http.put('/api/SourceFiles/' + $scope.selectedFile.id, { id: $scope.selectedFile.id, FileName: $scope.selectedFile.FileName, FileBody: $scope.selectedFile.FileBody })
            .success(function (data, status, headers, config) {
                $scope.ListFiles();

                // Reselect the file
                angular.forEach($scope.sourceFileData, function (value, index) {
                    if(value.id == $scope.selectedFile.id)
                    {
                        $scope.selectFile(value);
                    }
                })
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.parseFile = function () {
        $http.post('/HardwareSimulator/ParseFile', { id: $scope.selectedFile.id })
            .success(function (data, status, headers, config) {
                $scope.parsedFile = data;
            })
            .error(function (data, status, headers, config) {

            });
    };

    $scope.processFile = function () {
        $http.post('/HardwareSimulator/ProcessChip', { fileid: $scope.selectedFile.id, inputIDs: $scope.parsedFile.inputs, inputVals: $scope.inputValues })
            .success(function (data, status, headers, config) {
                $scope.outputValues = JSON.parse(data);
                $scope.fileProcessed = true;
            })
            .error(function (data, status, headers, config) {

            });
    };


    $scope.selectFile = function (file) {
        $scope.selectedFile = file;
        $scope.inputValues = [];
        $scope.fileProcessed = false;
        $scope.parseFile();
    };

    $scope.ListFiles();

}]);



