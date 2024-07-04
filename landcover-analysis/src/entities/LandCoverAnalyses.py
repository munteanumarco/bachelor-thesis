from sqlalchemy import Column, Integer, DateTime, Text
from sqlalchemy.dialects.postgresql import UUID
from sqlalchemy.sql.schema import ForeignKey
from sqlalchemy.ext.declarative import declarative_base

Base = declarative_base()

class LandCoverAnalyses(Base):
    __tablename__ = 'LandCoverAnalyses'
    
    Id = Column(UUID(as_uuid=True), primary_key=True)
    EmergencyEventId = Column(UUID(as_uuid=True), nullable=False)
    Status = Column(Integer, nullable=False)
    TriggeredAt = Column(DateTime(timezone=True), nullable=True)
    CompletedAt = Column(DateTime(timezone=True), nullable=True)
    ProcessedImage = Column(Text, nullable=True)
    RawImage = Column(Text, nullable=True)
