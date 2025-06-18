import { BehaviorSubject, map, Observable } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";


@Injectable()
export class UserService
{
    private apiUrl = '/api/users';
    private http = inject(HttpClient);
    private userSubject = new BehaviorSubject<any[]>([]);
    users$:Observable<any[]> = this.userSubject.asObservable();

    addUser(userData : any) : Observable<any>{
        return this.http.post(`${this.apiUrl}/add`, {userData});
    }

    fetchData() : void{
        this.http.get<any>(this.apiUrl).pipe(map((response: { users: any; }) => response.users)).subscribe({
            next : (data : any) => {
                this.userSubject.next(data)
            },
            error : (err) => {
                console.log(err);
            }
        })
    }
}