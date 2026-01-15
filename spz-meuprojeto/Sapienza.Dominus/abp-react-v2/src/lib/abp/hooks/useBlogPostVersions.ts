import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetBlogPostVersionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  blogPostId?: string;
  }

export function useBlogPostVersions(input: GetBlogPostVersionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["blogPostVersions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-post-version", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllBlogPostVersions() {
  return useQuery({
    queryKey: ["blogPostVersions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-post-version", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useBlogPostVersion(id: string) {
  return useQuery({
    queryKey: ["blogPostVersion", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/blog-post-version/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateBlogPostVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/blog-post-version", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogPostVersions"] });
    },
  });
}

export function useUpdateBlogPostVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/blog-post-version/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["blogPostVersions"] });
      queryClient.invalidateQueries({ queryKey: ["blogPostVersion", data.id] });
    },
  });
}

export function useDeleteBlogPostVersion() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/blog-post-version/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogPostVersions"] });
    },
  });
}


