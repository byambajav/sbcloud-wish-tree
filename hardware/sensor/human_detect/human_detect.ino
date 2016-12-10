const int ledPin = 13;
const int analogInPin = A0;
int   ad;

void setup()
{
  pinMode(ledPin, OUTPUT);
}

void loop()
{
  ad = analogRead(analogInPin);
  if ( ad == 0 ) {
    digitalWrite(ledPin, LOW);
  }else{
    digitalWrite(ledPin, HIGH);
  }
}

