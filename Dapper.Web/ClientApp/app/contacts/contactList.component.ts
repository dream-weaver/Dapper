import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Contact } from "../shared/contact";

@Component({
    selector: "contact-list",
    templateUrl: "contactList.component.html",
    styleUrls: []
})

export class contactListComponent implements OnInit {
    constructor(private data: DataService) {
    }
    public contacts: Contact[] = [];

    ngOnInit() {
        this.data.loadContacts()
            .subscribe(success => {
                if (success) {
                    this.contacts = this.data.contacts;
                }
            });
    }
}