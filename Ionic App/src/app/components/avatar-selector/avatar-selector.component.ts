import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-avatar-selector',
  templateUrl: './avatar-selector.component.html',
  styleUrls: ['./avatar-selector.component.scss'],
})

export class AvatarSelectorComponent implements OnInit {

  @Input() current = 'av-1.png'; // default value (registro)
  @Output() avatarSelected = new EventEmitter<string>();

  avatars = [
    {
      img: 'av-1.png',
      seleccionado: true
    },
    {
      img: 'av-2.png',
      seleccionado: false
    },
    {
      img: 'av-3.png',
      seleccionado: false
    },
    {
      img: 'av-4.png',
      seleccionado: false
    }
  ];

  constructor() { }

  ngOnInit() {
    this.avatars.forEach(av => av.seleccionado = (av.img === this.current) );
  }

  seleccionarAvatar(avatar: any) {
    this.avatars.forEach(av => av.seleccionado = false);
    avatar.seleccionado = true;
    this.avatarSelected.emit(avatar.img);
  }

}
