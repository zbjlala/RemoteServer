import { Component } from '@angular/core';
import { AppService } from '../app.service';
import { ServerModel, ServerModelItem } from 'src/interface/ServerModel';
import { ActivatedRoute, Router } from '@angular/router';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  //private app :AppService;
  serverList: ServerModel[];


  tableData: ServerModelItem[] = [];


  selected_ip = "127.0.1.1";
  check_result;


  constructor(private app: AppService, private router: Router) {
    //  this.app.getMsg();

    this.app.getServerList().subscribe(list => {

      //this.serverList =  Object.keys(list);
      this.serverList = list;

      list.forEach(it => {
        this.tableData.push({ ...it, msg: '' });
      })

      console.log(this.tableData)
    })


  }

  onSelect(serverip: string,type:string) {

    this.router.navigate(['/serveruserlist'],{queryParams:{serverip:serverip,type:type}});

  }


  selectChange(serverip) {
    this.selected_ip = serverip;
    console.log('您选择的是: ' + this.selected_ip);
  }

  checkKeyUp(server: ServerModelItem, value) {
    console.log('ip' + server.serverIP + 'value' + value);
    if(server.serverIP == value)
    {
      server.msg = 'SeverIP与TargetIP不能相等';
      return
    }
    this.app.checkTwoServerStatus(server.serverIP, value).subscribe(result => {
      console.log('返回值为' + result);
      // this.check_result = result.msg;

      server.msg = result.msg;
    })

  }

  reConnectkKeyUp(server: ServerModelItem, value) {
    console.log('ip' + server.serverIP + 'value' + value);
    if(server.serverIP == value)
    {
      server.msg = 'SeverIP与TargetIP不能相等';
      return
    }
    this.app.reConnectTwoServer(server.serverIP,value).subscribe(result =>{
      server.msg = result.msg
    });


  }



}
