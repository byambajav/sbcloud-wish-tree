const int ledPin = 13;
const int redLedPin = 12;
const int analogInPin = A0;
int   ad;

int prevState = 0;
const int switchPin = 8;
const int delayTimer = 5000;
const String humanMsg = "HUMAN";



void setup()
{
  pinMode(ledPin, OUTPUT);
  pinMode(redLedPin, OUTPUT);
  pinMode(switchPin, INPUT);
  Serial.begin(9600);
  delay(delayTimer);
  blinkLed();
}

void loop()
{
//  if(digitalRead(9) == HIGH){
//    if(prevState == 0){
//      Serial.println("PRESS");
//      prevState = 1;
//    }
//  } else {
//    if(prevState == 1){
//      Serial.println(" ");
//      prevState = 0;
//    }
//  }

//  if(digitalRead(switchPin) == HIGH){

    // reading human sensor
    ad = analogRead(analogInPin);
    if ( ad == 0 ) {
      digitalWrite(ledPin, LOW);
      if(prevState == 1){
        Serial.println(" ");
        prevState = 0;
      }
    }else{
      digitalWrite(ledPin, HIGH);
      if(prevState == 0){
        Serial.println(humanMsg);
        blinkLed();
        blinkLed();
        prevState = 1;
      }
    }
//  }
  

}

void blinkLed(){
  digitalWrite(redLedPin, HIGH);
  delay(100);
  digitalWrite(redLedPin, LOW);
  delay(100);
}

