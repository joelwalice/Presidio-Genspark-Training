export class RecruiterRegisterModel {
  constructor(
    public name: string = "",
    public email: string = "",
    public password: string = "",
    public cpassword: string = "",
    public phoneNumber: string = "",
    public address: string = "",
    public dateOfBirth: string = "",
    public companyName: string = ""
  ) {}
}
