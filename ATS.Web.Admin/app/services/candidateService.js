(function () {
    "use strict";

    angular.module("app.admin.portal")
           .factory("candidateService", candidateService);

    candidateService.$inject = ["$q", "$resource", "appSettings", "$http"];

    function candidateService($q, $resource, appSettings, $http) {
        function createResource(command) {
            var address = appSettings.apiEndpoint + "/api/candidates/" + command;
            return $resource(address);
        }

        return {
            getCandidates: function (pageSize, pageNumber, search) {
                var params = {};

                if (pageSize !== undefined && pageNumber !== undefined) {
                    params["$skip"] = (pageNumber - 1) * pageSize;
                    params["$top"] = pageSize;

                    if (search) {
                        if (search.email) {
                            params["$filter"] = "contains(Email eq tolower('" + search.email + "'))";
                        }

                        if (search.name) {
                            params["$filter"] = (params["$filter"] ? params["$filter"] + " and " : "") + "(contains(FirstName, tolower('" + search.name + "')) or contains(LastName, tolower('" + search.name + "')))";
                        }

                        if (search.state) {
                            params["$filter"] = (params["$filter"] ? params["$filter"] + " and " : "") + "contains(StateOfOrigin, tolower('" + search.state + "'))";
                        }

                        if (search.age !== undefined && search.age !== null) {
                            params["$filter"] = (params["$filter"] ? params["$filter"] + " and " : "") + "(Age " + search.direction + " " + search.age + ")";
                        }

                        params["$orderby"] = "CandidateId desc";
                    }
                }

                params["$count"] = true;

                return createResource("query").get(params).$promise;
            },

            getCandidateById: function (candidateId) {
                return createResource(":candidateId").get({ candidateId: candidateId }).$promise;
            },

            saveCandidate: function (candidate) {
                var operation = "edit";

                if (!candidate.candidateId) {
                    operation = "add";
                    delete candidate.candidateId;
                }

                return createResource(operation).save(candidate, function (data) {
                    return data;
                }).$promise;
            },

            deleteCandidate: function (candidateId) {
                return createResource("delete/" + candidateId).get({ candidateId: candidateId }).$promise;
            },

            getStates: function () {
                return createResource("states").query(function (data) {
                    return data;
                }).$promise;
            },
            downloadCandidateResume: function (candidateId) {
                var deferred = $q.defer();
                var candidateResumeLink = appSettings.apiEndpoint + "/api/candidates/preview/" + candidateId;

                $http.get(candidateResumeLink, { responseType: 'arraybuffer' })
                     .success(function (data, status, headers) {
                         var octetStreamMime = 'application/octet-stream';
                         var success = false;

                         headers = headers();

                         var filename = headers['x-ats-file'] || 'download.bin';

                         // Determine the content type from the header or default to "application/octet-stream"
                         var contentType = headers['content-type'] || octetStreamMime;

                         var ex, blob;
                         try {
                             console.log(filename);
                             // Try using msSaveBlob if supported
                             console.log("Trying saveBlob method ...");
                             blob = new Blob([data], { type: contentType });
                             if (navigator.msSaveBlob)
                                 navigator.msSaveBlob(blob, filename);
                             else {
                                 // Try using other saveBlob implementations, if available
                                 var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                                 if (saveBlob === undefined) throw "Not supported";
                                 saveBlob(blob, filename);
                             }
                             console.log("saveBlob succeeded");
                             success = true;
                         } catch (ex) {
                             console.log("saveBlob method failed with the following exception:");
                             console.log(ex);
                         }

                         if (!success) {
                             // Get the blob url creator
                             var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                             if (urlCreator) {
                                 // Try to use a download link
                                 var link = document.createElement('a');
                                 var url;
                                 if ('download' in link) {
                                     // Try to simulate a click
                                     try {
                                         // Prepare a blob URL
                                         console.log("Trying download link method with simulated click ...");
                                         blob = new Blob([data], { type: contentType });
                                         url = urlCreator.createObjectURL(blob);
                                         link.setAttribute('href', url);

                                         // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                         link.setAttribute("download", filename);

                                         // Simulate clicking the download link
                                         var event = document.createEvent('MouseEvents');
                                         event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                                         link.dispatchEvent(event);
                                         console.log("Download link method with simulated click succeeded");
                                         success = true;
                                     } catch (ex) {
                                         console.log("Download link method with simulated click failed with the following exception:");
                                         console.log(ex);
                                     }
                                 }

                                 if (!success) {
                                     // Fallback to window.location method
                                     try {
                                         // Prepare a blob URL
                                         // Use application/octet-stream when using window.location to force download
                                         console.log("Trying download link method with window.location ...");
                                         blob = new Blob([data], { type: octetStreamMime });
                                         url = urlCreator.createObjectURL(blob);
                                         window.location = url;
                                         console.log("Download link method with window.location succeeded");
                                         success = true;
                                     } catch (ex) {
                                         console.log("Download link method with window.location failed with the following exception:");
                                         console.log(ex);
                                     }
                                 }
                             }
                         }

                         if (!success) {
                             // Fallback to window.open method
                             console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                             window.open(candidateResumeLink, '_blank', '');
                         }

                         if (success) {
                             deferred.resolve("SUCCESS");
                         } else {
                             deferred.reject("NOT_SUPPORTED");
                         }
                         /******************/
                     }).error(function (data, status) {
                         console.log("Request failed with status: " + status);
                         deferred.reject("ERROR");
                     });

                return deferred.promise;
            }
        }
    }
}());
