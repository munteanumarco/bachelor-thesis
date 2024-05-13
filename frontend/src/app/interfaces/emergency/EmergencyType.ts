enum EmergencyType {
  FLOOD = 1,
  FIRE = 2,
  EARTHQUAKE = 3,
  HURRICANE = 4,
  TORNADO = 5,
  TSUNAMI = 6,
  VOLCANO = 7,
  DROUGHT = 8,
  AVALANCHE = 9,
}

const emergencyTypeNames: { [key in EmergencyType]: string } = {
  [EmergencyType.AVALANCHE]: 'Avalanche',
  [EmergencyType.DROUGHT]: 'Drought',
  [EmergencyType.EARTHQUAKE]: 'Earthquake',
  [EmergencyType.FIRE]: 'Fire',
  [EmergencyType.FLOOD]: 'Flood',
  [EmergencyType.HURRICANE]: 'Hurricane',
  [EmergencyType.TORNADO]: 'Tornado',
  [EmergencyType.TSUNAMI]: 'Tsunami',
  [EmergencyType.VOLCANO]: 'Volcano',
};

const getEmergencyTypeName = (emergencyType: EmergencyType): string =>
  emergencyTypeNames[emergencyType];

export { EmergencyType, getEmergencyTypeName, emergencyTypeNames };
