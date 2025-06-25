export class UserRegisterModel {
    constructor(public name: string = "", public email: string = "", public password: string = "", public cpassword: string = "", public phoneNumber: string = "", public dateOfBirth: Date = new Date(), public address: string = "") {

    }
}