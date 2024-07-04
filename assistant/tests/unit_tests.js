const chai = require("chai");
const chaiHttp = require("chai-http");
const app = require("../app");
const expect = chai.expect;

chai.use(chaiHttp);

describe("API Routes", () => {
  describe("GET /test", () => {
    it("should return status 'up'", (done) => {
      chai.request(app)
        .get("/test")
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body.status).to.equal("up");
          done();
        });
    });
  });

  describe("POST /assistants", () => {
    it("should create an assistant", (done) => {
      chai.request(app)
        .post("/assistants")
        .set('Authorization', 'Bearer fake-jwt-token')
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('object');
          done();
        });
    });
  });

  describe("POST /threads", () => {
    it("should create a thread", (done) => {
      chai.request(app)
        .post("/threads")
        .set('Authorization', 'Bearer fake-jwt-token') 
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('object');
          done();
        });
    });
  });

  describe("GET /threads/:threadId/messages", () => {
    it("should retrieve messages for a thread", (done) => {
      chai.request(app)
        .get("/threads/123/messages")
        .set('Authorization', 'Bearer fake-jwt-token')
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('array');
          done();
        });
    });
  });

  describe("DELETE /threads/:threadId", () => {
    it("should delete a thread", (done) => {
      chai.request(app)
        .delete("/threads/123")
        .set('Authorization', 'Bearer fake-jwt-token') 
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('object');
          done();
        });
    });
  });

  describe("DELETE /assistants/:assistantId", () => {
    it("should delete an assistant", (done) => {
      chai.request(app)
        .delete("/assistants/456")
        .set('Authorization', 'Bearer fake-jwt-token')
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('object');
          done();
        });
    });
  });

  describe("POST /assistants/:assistantId/threads/:threadId", () => {
    it("should create a message in a thread", (done) => {
      chai.request(app)
        .post("/assistants/456/threads/123")
        .set('Authorization', 'Bearer fake-jwt-token')
        .send({ userQuestion: 'What is the weather today?' })
        .end((err, res) => {
          expect(res).to.have.status(200);
          expect(res.body).to.be.an('object');
          done();
        });
    });
  });
});
