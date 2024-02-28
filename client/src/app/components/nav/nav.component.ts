import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent implements OnInit {
  model: any = {};
  @Output() clickEvent = new EventEmitter<boolean>();
  isClicked: boolean = false;
  constructor() {}

  ngOnInit(): void {}

  login() {
    console.log(this.model);
  }

  toggleButtonClick(): void {
    this.isClicked = true;
    this.clickEvent.emit(this.isClicked);
  }
}
