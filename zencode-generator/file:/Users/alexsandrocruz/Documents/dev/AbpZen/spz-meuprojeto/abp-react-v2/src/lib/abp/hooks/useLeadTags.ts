import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadTagsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useLeadTags(input: GetLeadTagsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadTags", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-tag", {
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

export function useAllLeadTags() {
  return useQuery({
    queryKey: ["leadTags", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-tag", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadTag(id: string) {
  return useQuery({
    queryKey: ["leadTag", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-tag/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadTag() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-tag", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadTags"] });
    },
  });
}

export function useUpdateLeadTag() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-tag/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadTags"] });
      queryClient.invalidateQueries({ queryKey: ["leadTag", data.id] });
    },
  });
}

export function useDeleteLeadTag() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-tag/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadTags"] });
    },
  });
}


