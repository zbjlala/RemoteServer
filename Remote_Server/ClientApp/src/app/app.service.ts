import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ServerModel } from 'src/interface/ServerModel';
import { UserSessionList } from 'src/interface/UserSessionList';
import { ResponseCode } from 'src/interface/ResponseCode';


@Injectable({
  providedIn: 'root'
})
export class AppService {

  app_name = 'app_test'

  constructor(private http: HttpClient,@Inject('BASE_URL') private baseUrl: string) {

  }


  getMsg() {

    const url = this.baseUrl + 'api/SampleData/WeatherForecasts';

    this.http.get(url).subscribe(data => {
      console.log(data)
    })
    
  }
  getServerList()
  {
    const url = this.baseUrl + 'api/ServerStatus/GetServerList';
    this.http.get<ServerModel[]>(url).subscribe(data =>{
      console.log(data)
      //return data;
    })
    return this.http.get<ServerModel[]>(url)
    
  }

  getUserListByIP(serverIP:string)
  {
    const url = this.baseUrl + 'api/ServerStatus/GetUserListByServerID';
    return this.http.post<UserSessionList[]>(url,{serverIP:serverIP}) 
  }

  checkTwoServerStatus(testIP:string,targetIP:string)
  {
    const url = this.baseUrl + 'api/ServerOption/CheckTwoServerStatus';
    return this.http.post<ResponseCode>(url,{testIP:testIP,targetIP:targetIP}) 
  }

  reConnectTwoServer(testIP:string,targetIP:string)
  {
    const url = this.baseUrl + 'api/ServerOption/ReConnectTwoServer';
    return this.http.post<ResponseCode>(url,{testIP:testIP,targetIP:targetIP});
  }
  getUserByID(userid:string)

  {
    const url = this.baseUrl + 'api/ServerStatus/GetUserByID';
    return this.http.post<UserSessionList[]>(url,{userID:userid})
  }



  getTips(data=null) {
    const url = this.baseUrl + 'api/SampleData/WeatherForecasts';

    return this.http.get(url, {params: data})
  }
}
