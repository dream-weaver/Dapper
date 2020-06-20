export interface Contact {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    company: string;
    title: string;
    isNew: boolean;
    addresses: Address[];
}
    export interface Address {
    id: number;
    contactId: number;
    addressType: string;
    streetAddress: string;
    city: string;
    stateId: number;
    postalCode: string;
    isNew: boolean;
    isDeleted: boolean;
}

