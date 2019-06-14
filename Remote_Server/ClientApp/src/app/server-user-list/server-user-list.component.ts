import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerModel } from 'src/interface/ServerModel';
import { AppService } from '../app.service';
import { UserSessionList } from 'src/interface/UserSessionList';

@Component({
  selector: 'app-server-user-list',
  templateUrl: './server-user-list.component.html',
  styleUrls: ['./server-user-list.component.scss']
})
export class ServerUserListComponent implements OnInit {

  ip: string;
  userlist: UserSessionList[];
  type: string;

  constructor(private app: AppService, private route: ActivatedRoute) {

  }

  ngOnInit() {

    this.route.queryParams.subscribe((data) => {

      console.log('当前的参数:' + data.serverip + "type" + data.type)
      this.ip = data.serverip;
      this.type = data.type;
    })


    if (this.type == "1") {
      this.app.getUserListByIP(this.ip).subscribe(list => {
        console.log(list);
        this.userlist = list;
      })
    }
    else {
      this.app.getUserByID(this.ip).subscribe(list => {
        console.log(list);
        this.userlist = list;
      }

      )
    }
  }




}
