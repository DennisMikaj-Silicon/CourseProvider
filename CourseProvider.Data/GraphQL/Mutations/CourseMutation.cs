using CourseProvider.Data.Models;
using CourseProvider.Data.Services;

namespace CourseProvider.Data.GraphQL.Mutations;

public class CourseMutation(ICourseService courseService)
{
    private readonly ICourseService _courseService = courseService;

    [GraphQLName("CreateCourse")]
    public async Task<Course> CreateCourseAsync(CourseCreateRequest input)
    {
        return await _courseService.CreateCourseAsync(input);
    }

    [GraphQLName("UpdateCourse")]
    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest input)
    {
        return await _courseService.UpdateCourseAsync(input);
    }

    [GraphQLName("DeleteCourse")]
    public async Task<bool> DeleteCourseAsync(string id)
    {
        return await _courseService.DeleteCourseAsync(id);
    }
}
