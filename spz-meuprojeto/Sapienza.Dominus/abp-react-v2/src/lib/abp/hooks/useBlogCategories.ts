import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetBlogCategoriesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useBlogCategories(input: GetBlogCategoriesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["blogCategories", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-category", {
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

export function useAllBlogCategories() {
  return useQuery({
    queryKey: ["blogCategories", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/blog-category", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useBlogCategory(id: string) {
  return useQuery({
    queryKey: ["blogCategory", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/blog-category/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateBlogCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/blog-category", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogCategories"] });
    },
  });
}

export function useUpdateBlogCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/blog-category/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["blogCategories"] });
      queryClient.invalidateQueries({ queryKey: ["blogCategory", data.id] });
    },
  });
}

export function useDeleteBlogCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/blog-category/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["blogCategories"] });
    },
  });
}


