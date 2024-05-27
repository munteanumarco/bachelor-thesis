const OpenAI = require("openai");

function getMessageParamsTemplate() {
  return {
    threadId: "",
    userQuestion: "",
    assistantId: "",
  };
}

async function createMessage(params) {
  const secretKey = process.env.OPENAI_API_KEY;
  const openai = new OpenAI({
    apiKey: secretKey,
  });

  const userQuestion = params.userQuestion;
  const inputThreadId = params.threadId;
  const inputAssistantId = params.assistantId;

  try {
    const messageResponse = await openai.beta.threads.messages.create(
      inputThreadId,
      {
        role: "user",
        content: userQuestion,
      }
    );
    console.log("Message Response:", messageResponse);

    const run = await openai.beta.threads.runs.create(inputThreadId, {
      assistant_id: inputAssistantId,
    });
    console.log("Run Creation Response:", run);

    let runStatus = await openai.beta.threads.runs.retrieve(
      inputThreadId,
      run.id
    );
    console.log("Initial Run Status:", runStatus);

    let attempts = 0;
    const maxAttempts = 20;
    const timeoutWaitTimeMs = 2000;

    while (runStatus.status !== "completed" && attempts < maxAttempts) {
      if (runStatus.status === "failed") {
        console.error("Run failed with details:", runStatus);
        break; // Exit the loop if the run has failed
      }

      await new Promise((resolve) => setTimeout(resolve, timeoutWaitTimeMs));
      runStatus = await openai.beta.threads.runs.retrieve(
        inputThreadId,
        run.id
      );
      attempts++;
      console.log(
        `Attempt ${attempts} of ${maxAttempts}: Status: ${runStatus.status}`
      );
    }

    const finalMessages = await openai.beta.threads.messages.list(
      inputThreadId
    );
    console.log("Final Messages:", finalMessages);
    return finalMessages;
  } catch (error) {
    console.error("Error occurred:", error);
  }
}

module.exports = { createMessage, getMessageParamsTemplate };
