const int ledPin = 13;
const int analogInPin = A0;
int   ad;

void setup()
{
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
}

void loop()
{
  ad = analogRead(analogInPin);
  if ( ad == 0 ) {
    digitalWrite(ledPin, LOW);
    Serial.println(" ");
  }else{
    digitalWrite(ledPin, HIGH);
    Serial.println("1");
  }
}

