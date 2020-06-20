import { __decorate } from "tslib";
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
let DataService = /** @class */ (() => {
    let DataService = class DataService {
        constructor(http) {
            this.http = http;
            this.contacts = [];
        }
        loadContacts() {
            return this.http.get("/api/addresses")
                .pipe(map((data) => {
                this.contacts = data;
                console.log(data);
                return true;
            }));
        }
    };
    DataService = __decorate([
        Injectable()
    ], DataService);
    return DataService;
})();
export { DataService };
//# sourceMappingURL=dataService.js.map