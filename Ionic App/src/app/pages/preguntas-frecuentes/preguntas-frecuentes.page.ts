import { Component, OnInit } from '@angular/core';
import { FrequentQuestion } from '../../interfaces/frequent-questions.interface';
import { PreguntasFrecuentesService } from '../../services/preguntas-frecuentes.service';

@Component({
  selector: 'app-preguntas-frecuentes',
  templateUrl: './preguntas-frecuentes.page.html',
  styleUrls: ['./preguntas-frecuentes.page.scss'],
})
export class PreguntasFrecuentesPage implements OnInit {

  // Variables
  frequentQuestions: FrequentQuestion[] = [];

  constructor(
    private frequentQuestionsService: PreguntasFrecuentesService
  ) { }

  ngOnInit() {
    this.frequentQuestionsService.leerPreguntasFrecuentes().subscribe(resp => { this.frequentQuestions = resp });
  }
}
