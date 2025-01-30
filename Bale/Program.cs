using static Bale.Bindings.GL;
using static Bale.Bindings.GLFW;
using static Bale.Bindings.Common;

if (!glfwInit()) {
    Console.WriteLine("failed to initialize glfw");
}

glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

var window = glfwCreateWindow(600, 400, "i hate C#", NULL, NULL);
if (window == NULL) {
    Console.WriteLine("failed to create window");
    glfwTerminate();
}

glfwMakeContextCurrent(window);

while (!glfwWindowShouldClose(window)) {
    glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    glClear(0x00004000);
    
    glfwPollEvents();
    glfwSwapBuffers(window);
}

glfwTerminate();