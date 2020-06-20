import { __decorate } from "tslib";
import { Component } from "@angular/core";
let contactListComponent = /** @class */ (() => {
    let contactListComponent = class contactListComponent {
        constructor(data) {
            this.data = data;
            this.contacts = [];
        }
        ngOnInit() {
            this.data.loadContacts()
                .subscribe(success => {
                if (success) {
                    this.contacts = this.data.contacts;
                }
            });
        }
    };
    contactListComponent = __decorate([
        Component({
            selector: "contact-list",
            templateUrl: "contactList.component.html",
            styleUrls: []
        })
    ], contactListComponent);
    return contactListComponent;
})();
export { contactListComponent };
//# sourceMappingURL=contactList.component.js.map