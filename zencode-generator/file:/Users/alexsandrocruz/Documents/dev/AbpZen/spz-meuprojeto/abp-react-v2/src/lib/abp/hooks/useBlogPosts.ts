import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetBlogPostsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  siteId?: string;
  }

export function useBlogPosts(input: GetBlogPostsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["blogPosts", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-post", {
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

export function useAllBlogPosts() {
  return useQuery({
    queryKey: ["blogPosts", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-post", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useBlogPost(id: string) {
  return useQuery({
    queryKey: ["blogPost", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/blog-post/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateBlogPost() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/blog-post", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogPosts"] });
    },
  });
}

export function useUpdateBlogPost() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/blog-post/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["blogPosts"] });
      queryClient.invalidateQueries({ queryKey: ["blogPost", data.id] });
    },
  });
}

export function useDeleteBlogPost() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/blog-post/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogPosts"] });
    },
  });
}


