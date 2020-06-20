import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';  
import { map } from 'rxjs/operators';
import { Contact } from "./contact";

@Injectable()
export class DataService {
    constructor(private http: HttpClient) { }
    public contacts: Contact[] = [];

    loadContacts(): Observable<boolean> {
        return this.http.get("/api/addresses")
            .pipe(map((data: any[]) => {
                this.contacts = data;
                console.log(data);
                return true;
            }));
    }

}