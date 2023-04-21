import http from 'k6/http';
import { sleep } from 'k6';
export const options = {
  vus: 10,
  iterations: 101,
};

export function setup() {
  // 2. setup code
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksAcousticAdventures');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSmoothJazzShowcase');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGrooveFest');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAlternativeAnthems');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTurntableTakeover');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLatinFusionFiesta');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSoulfulSerenade');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGlobalBeatsBonanza');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAll-NightIndieRager');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTechnoTango');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLiveLoopingExtravaganza');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSwinginSoiree');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGypsyJazzJam');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAmericanaJamboree');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTropicalTunesTreat');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLyricalLounge');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSaxophoneSpectacular');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGuitarGreatsGala');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAlternativeArena');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTranceTuneUp');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLostInTranslation');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSonicSymposium');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGlobalGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAcousticAmbrosia');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTechnicolorTrance');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLoudAndProud');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSoulfulSoundscape');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGenuineGig');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAuralAssault');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTheatreOfTrance');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLatinLovefest');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSensualSoiree');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGrungeGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAll-OutAlternative');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTechnoTempest');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLoudAndLyrical');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSaxophoneSoulstice');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGuitarGuruGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAcousticAria');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTranceTundra');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLostInTheGroove');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSonicSafari');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGlobalGrooveGetaway');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAlternativeApotheosis');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTranceTriumph');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLyricalLineup');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSmoothSensation');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGuitarGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAll-NightAcousticAttack');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTechnoTidalWave');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLatinLuminosity');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSonicSummit');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGlobalGiggle');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAltitudinousAnthems');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTheaterOfTechno');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLiveLoudly');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSoulfulSaxSalon');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGuitarGoddessGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAlternativeAscension');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTranceTsunami');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLostInTheMusic');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSonicSojourn');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGlobalGrooveGala');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAcousticAttitude');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTechnoTrench');
  // http.get('https://localhost:7042/beginticketsale?concertId=LoeksLyricalLuminaries');
  // http.get('https://localhost:7042/beginticketsale?concertId=StuartsSaxophoneSerenade');
  // http.get('https://localhost:7042/beginticketsale?concertId=GeertsGuitarGeniusGathering');
  // http.get('https://localhost:7042/beginticketsale?concertId=AlexsAltitudinousArena');
  // http.get('https://localhost:7042/beginticketsale?concertId=TroysTheaterOfTrancefusion');
}


export default function () {
  http.get('https://localhost:7042/buyTicket?quantity=1&concertid=taylorswift');
}
