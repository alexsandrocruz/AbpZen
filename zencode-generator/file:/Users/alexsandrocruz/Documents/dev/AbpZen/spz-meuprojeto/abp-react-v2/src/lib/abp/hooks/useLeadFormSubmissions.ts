import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadFormSubmissionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadFormId?: string;
  }

export function useLeadFormSubmissions(input: GetLeadFormSubmissionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leadFormSubmissions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form-submission", {
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

export function useAllLeadFormSubmissions() {
  return useQuery({
    queryKey: ["leadFormSubmissions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead-form-submission", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLeadFormSubmission(id: string) {
  return useQuery({
    queryKey: ["leadFormSubmission", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead-form-submission/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLeadFormSubmission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead-form-submission", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadFormSubmissions"] });
    },
  });
}

export function useUpdateLeadFormSubmission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead-form-submission/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leadFormSubmissions"] });
      queryClient.invalidateQueries({ queryKey: ["leadFormSubmission", data.id] });
    },
  });
}

export function useDeleteLeadFormSubmission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead-form-submission/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leadFormSubmissions"] });
    },
  });
}


