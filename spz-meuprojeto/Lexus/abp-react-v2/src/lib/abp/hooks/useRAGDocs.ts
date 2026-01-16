import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetRAGDocsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useRAGDocs(input: GetRAGDocsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["rAGDocs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ragdoc", {
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

export function useAllRAGDocs() {
  return useQuery({
    queryKey: ["rAGDocs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/ragdoc", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useRAGDoc(id: string) {
  return useQuery({
    queryKey: ["rAGDoc", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/ragdoc/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateRAGDoc() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/ragdoc", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["rAGDocs"] });
    },
  });
}

export function useUpdateRAGDoc() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/ragdoc/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["rAGDocs"] });
      queryClient.invalidateQueries({ queryKey: ["rAGDoc", data.id] });
    },
  });
}

export function useDeleteRAGDoc() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/ragdoc/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["rAGDocs"] });
    },
  });
}


