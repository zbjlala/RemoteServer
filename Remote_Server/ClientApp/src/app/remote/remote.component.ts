import { Component, OnInit } from '@angular/core';
import { AppService } from '../app.service';

@Component({
  selector: 'app-remote',
  templateUrl: './remote.component.html',
  styleUrls: ['./remote.component.css']
})
export class RemoteComponent implements OnInit {


  name = 'remote ------444';

  title = '';

  constructor(private app: AppService) {
    this.title = 'teett'


  }

  ngOnInit() {
    this.title = this.app.app_name;
  }

}
